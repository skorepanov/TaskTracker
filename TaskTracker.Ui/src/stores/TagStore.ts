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

    deleteTag = async (tag: ITag) => {
        const url = `${AppUrl}/tags/${tag.id}`;

        await Api.delete(url);

        runInAction(() => {
            this.tags = this.tags.filter(t => t.id !== tag.id);
        });
    };

    getSortedTags = () => {
        const tags = this.tags.slice();
        tags.sort((t1, t2) => t1.title.localeCompare(t2.title));
        return tags;
    };
}

export default TagStore;
