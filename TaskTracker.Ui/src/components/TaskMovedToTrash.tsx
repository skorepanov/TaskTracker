import React from 'react';
import ITaskMovedToTrash from '../interfaces/ITaskMovedToTrash';

class TaskMovedToTrash extends React.Component<ITaskMovedToTrashProps> {
    render() {
        const task = this.props.task;

        const description = task.description?.length > 0
            ? <><i>{task.description}</i><br /></>
            : null;

        const movedToTrashDateTime = task.movedToTrashDateTime
            ? <span>Дата перемещения в корзину: {task.movedToTrashDateTime}</span>
            : null;

        return (
            <div style={{ color: 'grey', marginBottom: 10 }}>
                [{task.id}] {task.title}<br />
                {description}
                {movedToTrashDateTime}
            </div>
        );
    }
}

interface ITaskMovedToTrashProps {
    task: ITaskMovedToTrash;
}

export default TaskMovedToTrash;
