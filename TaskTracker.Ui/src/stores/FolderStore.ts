import { makeAutoObservable, runInAction } from "mobx";
import { Api, AppUrl } from "../api";
import IFolder from "../interfaces/IFolder";

class FolderStore {
    folders: IFolder[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    fetchFolders = async () => {
        const url = `${AppUrl}/folders`;

        const folders = await Api.get<IFolder[]>(url);

        runInAction(() => {
            this.folders = folders;
        });
    };

    createFolder = async (title: string) => {
        const url = `${AppUrl}/folders`;

        const params = {
            title: title,
            createdDateTime: new Date().toISOString(),
        };

        const createdFolder = await Api.post<IFolder>(url, params);

        runInAction(() => {
            this.folders.push(createdFolder);
        });
    };

    deleteFolder = async (folder: IFolder) => {
        const url = `${AppUrl}/folders/${folder.id}`;

        await Api.delete(url);

        runInAction(() => {
            this.folders = this.folders.filter(f => f.id !== folder.id);
        });
    };
}

export default FolderStore;
