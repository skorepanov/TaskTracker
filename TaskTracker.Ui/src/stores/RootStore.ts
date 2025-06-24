import { createContext, useContext } from "react";
import TaskStore from "./TaskStore";
import FolderStore from "./FolderStore";
import TagStore from "./TagStore";
import ITag from "../interfaces/ITag";

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

    deleteTag = async (tag: ITag) => {
        await this.tagStore.deleteTag(tag);
        this.taskStore.removeTagFromTasks(tag.id);
    };
}

const rootStore = new RootStore();

const StoreContext = createContext<RootStore>(rootStore);

export const useStore = () => useContext(StoreContext);
