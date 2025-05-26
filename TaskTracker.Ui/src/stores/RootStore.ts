import { createContext, useContext } from "react";
import TaskStore from "./TaskStore";
import FolderStore from "./FolderStore";

class RootStore {
    taskStore: TaskStore;
    folderStore: FolderStore;

    constructor() {
        this.taskStore = new TaskStore();
        this.folderStore = new FolderStore();
    }
}

const rootStore = new RootStore();

const StoreContext = createContext<RootStore>(rootStore);

export const useStore = () => useContext(StoreContext);
