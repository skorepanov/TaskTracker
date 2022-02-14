import React from 'react';
import ITask from '../interfaces/ITask';

class Task extends React.Component<ITaskProps> {
    render() {
        const task = this.props.task;

        const textColor = task.completionDate !== null ? 'green' : 'black';

        const description = task.description?.length > 0
            ? <><i>{task.description}</i><br /></>
            : null;

        const completionDate = task.completionDate !== null
            ? <>Выполнено: {task.completionDate}<br /></>
            : null;

        const dueDate = task.dueDate !== null
            ? <span>Срок выполнения: {task.dueDate}</span>
            : null;

        return (
            <div style={{ color: textColor, marginBottom: 10 }}>
                [{task.id}] {task.title}<br />
                {description}
                {completionDate}
                {dueDate}
            </div>
        );
    }
}

interface ITaskProps {
    task: ITask;
}

export default Task;
