import React from 'react';
import ITask from '../interfaces/ITask';

class Task extends React.Component<ITaskProps> {
    render() {
        const task = this.props.task;

        return (
            <div>
                ID: {task.id}<br />
                Заголовок: {task.title}<br />
                Описание: {task.description}
            </div>
        );
    }
}

interface ITaskProps {
    task: ITask;
}

export default Task;
