import { makeAutoObservable, runInAction } from "mobx";
import dayjs, { Dayjs } from "dayjs";
import ITask from "../interfaces/ITask";
import { Api, AppUrl } from "../api";

class TaskStore {
    incompletedTasks: ITask[] = [];
    completedTasks: ITask[] = [];
    tasksMovedToTrash: ITask[] = [];

    currentTask?: ITask;

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

            if (this.currentTask?.id === task.id) {
                this.currentTask = undefined;
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

    getAllIncompletedTasks = () => {
        const tasks = this.incompletedTasks.slice();
        tasks.sort((t1, t2) => t1.title.localeCompare(t2.title));
        return tasks;
    };

    getAllCompletedTasks = () => {
        const tasks = this.completedTasks.slice();
        tasks.sort((t1, t2) => t1.title.localeCompare(t2.title));
        return tasks;
    };

    getTodayIncompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf("day");

        const tasks = this.incompletedTasks.filter(t =>
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

        const tasks = this.completedTasks.filter(t =>
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
        const tasks = this.incompletedTasks.filter(t => t.folderId === null);
        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getInboxCompletedTasks = () => {
        const tasks = this.completedTasks.filter(
            task => task.folderId === null
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getFolderIncompletedTasks = (folderId: number) => {
        const tasks = this.incompletedTasks.filter(
            t => t.folderId === folderId
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getFolderCompletedTasks = (folderId: number) => {
        const tasks = this.completedTasks.filter(t => t.folderId === folderId);
        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getTagIncompletedTasks = (tagId: number) => {
        const tasks = this.incompletedTasks.filter(t =>
            t.tagIds?.includes(tagId)
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getTagCompletedTasks = (tagId: number) => {
        const tasks = this.completedTasks.filter(t =>
            t.tagIds?.includes(tagId)
        );

        this.sortTasksByTitle(tasks);
        return tasks;
    };

    getTasksMovedToTrash = () => {
        const tasks = this.tasksMovedToTrash.slice();
        this.sortTasksByTitle(tasks);
        return tasks;
    };

    setCurrentTask = (task?: ITask) => {
        this.currentTask = task;
    };
}

export default TaskStore;
