import ITask from './ITask';

export default interface IDeletedTask extends ITask {
    deletionDate: Date,
}
