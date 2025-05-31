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
}

export default TagStore;
