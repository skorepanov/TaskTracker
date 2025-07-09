import { makeAutoObservable, runInAction, observable } from "mobx";
import dayjs, { Dayjs } from "dayjs";
import ITask from "../interfaces/ITask";
import { Api, AppUrl } from "../api";

class TaskStore {
    tasks = observable.map<number, ITask>();

    get taskArray(): ITask[] {
        return Array.from(this.tasks.values());
    }

    private _currentTaskId: number | null = null;

    get currentTask() {
        if (this._currentTaskId === null) {
            return null;
        }

        return this.tasks.get(this._currentTaskId);
    }

    set currentTaskId(taskId: number | null) {
        this._currentTaskId = taskId;
    }

    constructor() {
        makeAutoObservable(this);
    }

    fetchIncompleteTasks = async () => {
        const url = `${AppUrl}/tasks/incomplete`;
        const response = await Api.get<ITask[]>(url);

        if (!response.isOk || response.value === null) {
            return;
        }

        const tasks = response.value;

        runInAction(() => {
            tasks.forEach(t => {
                this.tasks.set(t.id, t);
            });
        });
    };

    fetchCompletedTasks = async () => {
        const url = `${AppUrl}/tasks/complete`;
        const response = await Api.get<ITask[]>(url);

        if (!response.isOk || response.value === null) {
            return;
        }

        const tasks = response.value;

        runInAction(() => {
            tasks.forEach(t => {
                this.tasks.set(t.id, t);
            });
        });
    };

    fetchTasksInTrash = async () => {
        const url = `${AppUrl}/tasks/trash`;

        const response = await Api.get<ITask[]>(url);

        if (!response.isOk || response.value === null) {
            return;
        }

        const tasks = response.value;

        runInAction(() => {
            tasks.forEach(t => {
                this.tasks.set(t.id, t);
            });
        });
    };

    createTask = async (
        title: string,
        dueDateTime: Date | null,
        folderId: number | null
    ) => {
        const url = `${AppUrl}/tasks`;

        const params = {
            title: title,
            dueDateTime: dueDateTime,
            folderId: folderId,
            createdDateTime: new Date().toISOString(),
        };

        const response = await Api.post<ITask>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const createdTask = response.value;

        runInAction(() => {
            this.tasks.set(createdTask.id, createdTask);
            this.currentTaskId = createdTask.id;
        });
    };

    updateTask = async (
        taskId: number,
        title: string,
        description: string | null,
        folderId: number | null,
        dueDateTime: Date | null,
        tagIds: number[] | null
    ) => {
        const url = `${AppUrl}/tasks/${taskId}`;

        const params = {
            title: title,
            description: description,
            folderId: folderId,
            dueDateTime: dueDateTime,
            tagIds: tagIds,
            modifiedDateTime: new Date().toISOString(),
        };

        const response = await Api.put<ITask>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const updatedTask = response.value;

        runInAction(() => {
            this.tasks.set(updatedTask.id, updatedTask);
        });
    };

    deleteTask = async (taskId: number) => {
        const url = `${AppUrl}/tasks/${taskId}`;

        const response = await Api.delete(url);

        if (!response.isOk) {
            return;
        }

        runInAction(() => {
            this.tasks.delete(taskId);

            if (this._currentTaskId === taskId) {
                this._currentTaskId = null;
            }
        });
    };

    completeTask = async (taskId: number) => {
        const url = `${AppUrl}/tasks/${taskId}/completed`;

        const params = {
            completedDateTime: new Date().toISOString(),
        };

        const response = await Api.put<ITask>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const updatedTask = response.value;

        runInAction(() => {
            this.tasks.set(updatedTask.id, updatedTask);
        });
    };

    incompleteTask = async (taskId: number) => {
        const url = `${AppUrl}/tasks/${taskId}/incompleted`;

        const params = {
            modifiedDateTime: new Date().toISOString(),
        };

        const response = await Api.put<ITask>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const updatedTask = response.value;

        runInAction(() => {
            this.tasks.set(updatedTask.id, updatedTask);
        });
    };

    moveTaskToTrash = async (taskId: number) => {
        const url = `${AppUrl}/tasks/${taskId}/movedToTrash`;

        const params = {
            movedToTrashDateTime: new Date().toISOString(),
        };

        const response = await Api.put<ITask>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const taskMovedToTrash = response.value;

        runInAction(() => {
            this.tasks.set(taskMovedToTrash.id, taskMovedToTrash);

            if (this._currentTaskId === taskId) {
                this._currentTaskId = null;
            }
        });
    };

    moveTaskFromTrash = async (taskId: number, folderId: number | null) => {
        const url = `${AppUrl}/tasks/${taskId}/movedFromTrash`;

        const params = {
            folderId: folderId,
            modifiedDateTime: new Date().toISOString(),
        };

        const response = await Api.put<ITask>(url, params);

        if (!response.isOk || response.value === null) {
            return;
        }

        const taskMovedFromTrash = response.value;

        runInAction(() => {
            this.tasks.set(taskMovedFromTrash.id, taskMovedFromTrash);

            if (this._currentTaskId === taskId) {
                this._currentTaskId = null;
            }
        });
    };

    getIncompletedTasks = (shouldSort: boolean = false) => {
        const tasks = this.taskArray.filter(
            t => t.movedToTrashDateTime === null && t.completedDateTime === null
        );

        if (shouldSort) {
            this.sortTasksByTitle(tasks);
        }

        return tasks;
    };

    getCompletedTasks = (shouldSort: boolean = false) => {
        const tasks = this.taskArray.filter(
            t => t.movedToTrashDateTime === null && t.completedDateTime !== null
        );

        if (shouldSort) {
            this.sortTasksByTitle(tasks);
        }

        return tasks;
    };

    getTodayIncompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf("day");

        const tasks = this.getIncompletedTasks().filter(t =>
            this.isTodayIncompletedTask(t, todayDate)
        );

        this.sortTasksByTitle(tasks);

        return tasks;
    };

    isTodayIncompletedTask = (task: ITask, todayDate: Dayjs) => {
        if (task.dueDateTime === null) {
            return false;
        }

        const dueDateTime = dayjs(task.dueDateTime).startOf("day");

        if (dueDateTime.isSame(todayDate) || dueDateTime.isBefore(todayDate)) {
            return true;
        }
    };

    private sortTasksByTitle = (tasks: ITask[]) => {
        tasks.sort((t1, t2) => t1.title.localeCompare(t2.title));
    };

    getTodayCompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf("day");

        const tasks = this.getCompletedTasks().filter(t =>
            this.isTodayCompletedTask(t, todayDate)
        );

        this.sortTasksByTitle(tasks);

        return tasks;
    };

    isTodayCompletedTask = (task: ITask, todayDate: Dayjs) => {
        const completedDateTime = dayjs(task.completedDateTime).startOf("day");

        if (completedDateTime.isSame(todayDate)) {
            return true;
        }
    };

    getInboxIncompletedTasks = () => {
        const tasks = this.getIncompletedTasks().filter(
            t => t.folderId === null
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getInboxCompletedTasks = () => {
        const tasks = this.getCompletedTasks().filter(t => t.folderId === null);
        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getFolderIncompletedTasks = (folderId: number) => {
        const tasks = this.getIncompletedTasks().filter(
            t => t.folderId === folderId
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getFolderCompletedTasks = (folderId: number) => {
        const tasks = this.getCompletedTasks().filter(
            t => t.folderId === folderId
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getTagIncompletedTasks = (tagId: number) => {
        const tasks = this.getIncompletedTasks().filter(t =>
            t.tagIds?.includes(tagId)
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getTagCompletedTasks = (tagId: number) => {
        const tasks = this.getCompletedTasks().filter(t =>
            t.tagIds?.includes(tagId)
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getTasksMovedToTrash = () => {
        const tasks = this.taskArray.filter(
            t => t.movedToTrashDateTime !== null
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    removeTasksFromFolder = (folderId: number) => {
        if (this.currentTask?.folderId === folderId) {
            this.currentTaskId = null;
        }

        for (const task of this.taskArray) {
            if (task.folderId === folderId) {
                this.tasks.delete(task.id);
            }
        }
    };

    removeTagFromTasks = (tagId: number) => {
        for (const task of this.taskArray) {
            if (task.tagIds !== null) {
                task.tagIds = task.tagIds.filter(t => t !== tagId);
            }
        }
    };
}

export default TaskStore;
