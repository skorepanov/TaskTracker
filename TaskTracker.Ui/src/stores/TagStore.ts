import { makeAutoObservable, runInAction } from "mobx";
import { Api, AppUrl } from "../api";
import ITag from "../interfaces/ITag";

class TagStore {
    tags: ITag[] = [];

    constructor() {
        makeAutoObservable(this);
    }

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
