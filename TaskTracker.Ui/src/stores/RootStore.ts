import { createContext, useContext } from "react";
import TaskStore from "./TaskStore";
import FolderStore from "./FolderStore";
import TagStore from "./TagStore";

class RootStore {
    taskStore: TaskStore;
    folderStore: FolderStore;
    tagStore: TagStore;

    constructor() {
        this.taskStore = new TaskStore();
        this.folderStore = new FolderStore();
        this.tagStore = new TagStore();
    }

    fetchInitialData = async () => {
        await Promise.all([
            this.taskStore.fetchIncompleteTasks(),
            this.taskStore.fetchCompletedTasks(),
            this.taskStore.fetchTasksInTrash(),
            this.folderStore.fetchFolders(),
            this.tagStore.fetchTags(),
        ]);
    };

    deleteFolder = async (folderId: number) => {
        await this.folderStore.deleteFolder(folderId);
        this.taskStore.removeTasksFromFolder(folderId);
    };

    deleteTag = async (tagId: number) => {
        await this.tagStore.deleteTag(tagId);
        this.taskStore.removeTagFromTasks(tagId);
    };
}

const rootStore = new RootStore();

const StoreContext = createContext<RootStore>(rootStore);

export const useStore = () => useContext(StoreContext);
