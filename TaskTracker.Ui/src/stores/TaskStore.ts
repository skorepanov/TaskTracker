import { makeAutoObservable, runInAction, observable } from "mobx";
import dayjs, { Dayjs } from "dayjs";
import ITask from "../interfaces/ITask";
import { Api, AppUrl } from "../api";

class TaskStore {
    tasks = observable.map<number, ITask>();

    get taskArray(): ITask[] {
        return Array.from(this.tasks.values());
    }

    private _currentTaskId: number | undefined;

    get currentTask() {
        if (this._currentTaskId === undefined) {
            return undefined;
        }

        return this.tasks.get(this._currentTaskId);
    }

    set currentTaskId(taskId: number | undefined) {
        this._currentTaskId = taskId;
    }

    constructor() {
        makeAutoObservable(this);
    }

    fetchIncompleteTasks = async () => {
        const url = `${AppUrl}/tasks/incomplete`;
        const tasks = await Api.get<ITask[]>(url);

        runInAction(() => {
            tasks.forEach(t => {
                this.tasks.set(t.id, t);
            });
        });
    };

    fetchCompletedTasks = async () => {
        const url = `${AppUrl}/tasks/complete`;
        const tasks = await Api.get<ITask[]>(url);

        runInAction(() => {
            tasks.forEach(t => {
                this.tasks.set(t.id, t);
            });
        });
    };

    fetchTasksInTrash = async () => {
        const url = `${AppUrl}/tasks/trash`;

        const tasks = await Api.get<ITask[]>(url);

        runInAction(() => {
            tasks.forEach(t => {
                this.tasks.set(t.id, t);
            });
        });
    };

    createTask = async (
        title: string,
        description: string,
        dueDateTime: Date | null,
        folderId: number | null
    ) => {
        const url = `${AppUrl}/tasks`;

        const params = {
            title: title,
            description: description,
            dueDateTime: dueDateTime,
            folderId: folderId,
            createdDateTime: new Date().toISOString(),
        };

        const createdTask = await Api.post<ITask>(url, params);

        runInAction(() => {
            this.tasks.set(createdTask.id, createdTask);
        });
    };

    updateTask = async (
        taskId: number,
        title: string,
        description: string | null,
        folderId: number | null,
        dueDateTime: Date | null
    ) => {
        const url = `${AppUrl}/tasks/${taskId}`;

        const params = {
            title: title,
            description: description,
            folderId: folderId,
            dueDateTime: dueDateTime,
            modifiedDateTime: new Date().toISOString(),
        };

        const updatedTask = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.tasks.set(updatedTask.id, updatedTask);
        });
    };

    deleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}`;

        await Api.delete(url);

        runInAction(() => {
            this.tasks.delete(task.id);

            if (this._currentTaskId === task.id) {
                this._currentTaskId = undefined;
            }
        });
    };

    completeTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/completed`;

        const params = {
            completedDateTime: new Date().toISOString(),
        };

        const updatedTask = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.tasks.set(updatedTask.id, updatedTask);
        });
    };

    incompleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/incompleted`;

        const params = {
            modifiedDateTime: new Date().toISOString(),
        };

        const updatedTask = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.tasks.set(updatedTask.id, updatedTask);
        });
    };

    moveTaskToTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedToTrash`;

        const params = {
            movedToTrashDateTime: new Date().toISOString(),
        };

        const taskMovedToTrash = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.tasks.set(taskMovedToTrash.id, taskMovedToTrash);

            if (this._currentTaskId === task.id) {
                this._currentTaskId = undefined;
            }
        });
    };

    moveTaskFromTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedFromTrash`;

        const params = {
            modifiedDateTime: new Date().toISOString(),
        };

        const taskMovedFromTrash = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.tasks.set(taskMovedFromTrash.id, taskMovedFromTrash);

            if (this._currentTaskId === task.id) {
                this._currentTaskId = undefined;
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
}

export default TaskStore;
