import { makeAutoObservable, runInAction } from "mobx";
import { Api, AppUrl } from "../api";
import ITag from "../interfaces/ITag";

class TagStore {
    tags: ITag[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    fetchTags = async () => {
        const url = `${AppUrl}/tags`;

        const tags = await Api.get<ITag[]>(url);

        runInAction(() => {
            this.tags = tags;
        });
    };

    createTag = async (title: string, color: string) => {
        const url = `${AppUrl}/tags`;

        const params = {
            title: title,
            color: color,
            createdDateTime: new Date().toISOString(),
        };

        const createdTag = await Api.post<ITag>(url, params);

        runInAction(() => {
            this.tags.push(createdTag);
        });
    };

    updateTag = async (tagId: number, title: string, color: string) => {
        const url = `${AppUrl}/tags/${tagId}`;

        const params = {
            title: title,
            color: color,
            modifiedDateTime: new Date().toISOString(),
        };

        const updatedTag = await Api.put<ITag>(url, params);

        runInAction(() => {
            this.tags = this.tags.map(t =>
                t.id === updatedTag.id ? updatedTag : t
            );
        });
    };

    deleteTag = async (tagId: number) => {
        const url = `${AppUrl}/tags/${tagId}`;

        await Api.delete(url);

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
