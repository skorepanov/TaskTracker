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

        const response = await Api.get<IFolder[]>(url);

        if (!response.isOk || response.result === null) {
            return;
        }

        const folders = response.result;

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

        const response = await Api.post<IFolder>(url, params);

        if (!response.isOk || response.result === null) {
            return;
        }

        const createdFolder = response.result;

        runInAction(() => {
            this.folders.push(createdFolder);
        });
    };

    updateFolder = async (folderId: number, title: string) => {
        const url = `${AppUrl}/folders/${folderId}`;

        const params = {
            title: title,
            modifiedDateTime: new Date().toISOString(),
        };

        const response = await Api.put<IFolder>(url, params);

        if (!response.isOk || response.result === null) {
            return;
        }

        const updatedFolder = response.result;

        runInAction(() => {
            this.folders = this.folders.map(f =>
                f.id === updatedFolder.id ? updatedFolder : f
            );
        });
    };

    deleteFolder = async (folderId: number) => {
        const url = `${AppUrl}/folders/${folderId}`;

        const response = await Api.delete(url);

        if (!response.isOk) {
            return;
        }

        runInAction(() => {
            this.folders = this.folders.filter(f => f.id !== folderId);
        });
    };

    getSortedFolders = () => {
        const folders = this.folders.slice();
        folders.sort((f1, f2) => f1.title.localeCompare(f2.title));
        return folders;
    };
}

export default FolderStore;
