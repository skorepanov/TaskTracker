import React from 'react';
import ITask from '../interfaces/ITask';

class Task extends React.Component<ITaskProps> {
    render() {
        const task = this.props.task;

        const textColor = task.completedDateTime !== null ? 'green' : 'black';

        const description = task.description?.length > 0
            ? <><i>{task.description}</i><br /></>
            : null;

        const completedDateTime = task.completedDateTime !== null
            ? <>Выполнено: {task.completedDateTime}<br /></>
            : null;

        const dueDateTime = task.dueDateTime !== null
            ? <span>Срок выполнения: {task.dueDateTime}</span>
            : null;

        return (
            <div style={{ color: textColor, marginBottom: 10 }}>
                [{task.id}] {task.title}<br />
                {description}
                {completedDateTime}
                {dueDateTime}
            </div>
        );
    }
}

interface ITaskProps {
    task: ITask;
}

export default Task;
