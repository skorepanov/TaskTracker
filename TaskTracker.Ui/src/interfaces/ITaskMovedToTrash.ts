import ITask from './ITask';

export default interface ITaskMovedToTrash extends ITask {
    movedToTrashDateTime: Date,
}
