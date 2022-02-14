import ITask from './ITask';

export default interface IFolder {
    id: number;
    title: string;
    incompleteTaskCount: number;
    tasks: ITask[] | null;
}
