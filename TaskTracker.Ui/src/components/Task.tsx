import React from 'react';
import ITask from '../interfaces/ITask';

class Task extends React.Component<ITaskProps> {
    render() {
        const task = this.props.task;
        const dueDate = task.dueDate !== null
            ? <span>Срок выполнения: {task.dueDate}</span>
            : null;

        return (
            <div>
                ID: {task.id}<br />
                Заголовок: {task.title}<br />
                Описание: {task.description}<br />
                {dueDate}
            </div>
        );
    }
}

interface ITaskProps {
    task: ITask;
}

export default Task;
