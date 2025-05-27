import { makeAutoObservable, runInAction } from "mobx";
import dayjs, { Dayjs } from "dayjs";
import ITask from "../interfaces/ITask";
import { Api, AppUrl } from "../api";

class TaskStore {
    incompletedTasks: ITask[] = [];
    completedTasks: ITask[] = [];
    tasksMovedToTrash: ITask[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    fetchIncompleteTasks = async () => {
        const url = `${AppUrl}/tasks/incomplete`;
        const tasks = await Api.get<ITask[]>(url);

        runInAction(() => {
            this.incompletedTasks = tasks;
        });
    };

    fetchCompletedTasks = async () => {
        const url = `${AppUrl}/tasks/complete`;
        const tasks = await Api.get<ITask[]>(url);

        runInAction(() => {
            this.completedTasks = tasks;
        });
    };

    fetchTasksInTrash = async () => {
        const url = `${AppUrl}/tasks/trash`;

        const tasks = await Api.get<ITask[]>(url);

        runInAction(() => {
            this.tasksMovedToTrash = tasks;
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
            this.incompletedTasks.push(createdTask);
        });
    };

    deleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}`;

        await Api.delete(url);

        runInAction(() => {
            this.tasksMovedToTrash = this.tasksMovedToTrash.filter(
                t => t.id !== task.id
            );
        });
    };

    completeTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/completed`;

        const params = {
            completedDateTime: new Date().toISOString(),
        };

        const updatedTask = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.incompletedTasks = this.incompletedTasks.filter(
                t => t.id !== updatedTask.id
            );

            this.completedTasks.push(updatedTask);
        });
    };

    incompleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/incompleted`;

        const params = {
            modifiedDateTime: new Date().toISOString(),
        };

        const updatedTask = await Api.put<ITask>(url, params);

        runInAction(() => {
            this.completedTasks = this.completedTasks.filter(
                t => t.id !== updatedTask.id
            );

            this.incompletedTasks.push(updatedTask);
        });
    };

    moveTaskToTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedToTrash`;

        const params = {
            movedToTrashDateTime: new Date().toISOString(),
        };

        const taskMovedToTrash = await Api.put<ITask>(url, params);

        runInAction(() => {
            if (taskMovedToTrash.completedDateTime !== null) {
                this.completedTasks = this.completedTasks.filter(
                    t => t.id !== taskMovedToTrash.id
                );
            } else {
                this.incompletedTasks = this.incompletedTasks.filter(
                    t => t.id !== taskMovedToTrash.id
                );
            }

            this.tasksMovedToTrash.push(taskMovedToTrash);
        });
    };

    moveTaskFromTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedFromTrash`;

        const params = {
            modifiedDateTime: new Date().toISOString(),
        };

        const taskMovedFromTrash = await Api.put<ITask>(url, params);

        runInAction(() => {
            if (taskMovedFromTrash.completedDateTime !== null) {
                this.completedTasks.push(taskMovedFromTrash);
            } else {
                this.incompletedTasks.push(taskMovedFromTrash);
            }

            this.tasksMovedToTrash = this.tasksMovedToTrash.filter(
                t => t.id !== taskMovedFromTrash.id
            );
        });
    };

    getTodayIncompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf("day");

        return this.incompletedTasks.filter(t =>
            this.isTodayIncompletedTask(t, todayDate)
        );
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

    getTodayCompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf("day");

        return this.completedTasks.filter(t =>
            this.isTodayCompletedTask(t, todayDate)
        );
    };

    isTodayCompletedTask = (task: ITask, todayDate: Dayjs) => {
        const completedDateTime = dayjs(task.completedDateTime).startOf("day");

        if (completedDateTime.isSame(todayDate)) {
            return true;
        }
    };

    getInboxIncompletedTasks = () => {
        return this.incompletedTasks.filter(t => t.folderId === null);
    };

    getInboxCompletedTasks = () => {
        return this.completedTasks.filter(task => task.folderId === null);
    };
}

export default TaskStore;
