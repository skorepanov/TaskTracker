import { makeAutoObservable, runInAction } from "mobx";
import { Api, ApiUrl } from "../api";
import ITag from "../interfaces/ITag";

class TagStore {
    tags: ITag[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    fetchTags = async () => {
        const url = `${ApiUrl}/tags`;

        const response = await Api.get<ITag[]>(url);

        if (!response.isOk || response.value === null) {
            return;
        }

        const tags = response.value;

        runInAction(() => {
            this.tags = tags;
        });
    };

    createTag = async (title: string, color: string) => {
        const url = `${ApiUrl}/tags`;

        const params = {
            title: title,
            color: color,
            createdDateTime: new Date().toISOString(),
        };

        const response = await Api.post<ITag>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const createdTag = response.value;

        runInAction(() => {
            this.tags.push(createdTag);
        });
    };

    updateTag = async (tagId: number, title: string, color: string) => {
        const url = `${ApiUrl}/tags/${tagId}`;

        const params = {
            title: title,
            color: color,
            modifiedDateTime: new Date().toISOString(),
        };

        const response = await Api.put<ITag>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const updatedTag = response.value;

        runInAction(() => {
            this.tags = this.tags.map(t =>
                t.id === updatedTag.id ? updatedTag : t
            );
        });
    };

    deleteTag = async (tagId: number) => {
        const url = `${ApiUrl}/tags/${tagId}`;

        const response = await Api.delete(url);

        if (!response.isOk) {
            return;
        }

        runInAction(() => {
            this.tags = this.tags.filter(t => t.id !== tagId);
        });
    };

    getSortedTags = () => {
        const tags = this.tags.slice();
        tags.sort((t1, t2) => t1.title.localeCompare(t2.title));
        return tags;
    };

    getFilteredSortedTags = (tagIds: number[] | null) => {
        if (tagIds === null || tagIds.length === 0) {
            return null;
        }

        const tags = this.tags.filter(t => tagIds.includes(t.id));
        tags.sort((t1, t2) => t1.title.localeCompare(t2.title));
        return tags;
    };
}

export default TagStore;
