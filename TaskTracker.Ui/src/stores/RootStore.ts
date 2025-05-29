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
}

const rootStore = new RootStore();

const StoreContext = createContext<RootStore>(rootStore);

export const useStore = () => useContext(StoreContext);
