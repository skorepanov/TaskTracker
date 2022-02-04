import React from 'react';
import ITask from '../interfaces/ITask';

class Task extends React.Component<ITaskProps> {
    render() {
        const task = this.props.task;

        return (
            <div>
                ID: {task.id}<br />
                Title: {task.title}<br />
                Description: {task.description}
            </div>
        );
    }
}

interface ITaskProps {
    task: ITask;
}

export default Task;
