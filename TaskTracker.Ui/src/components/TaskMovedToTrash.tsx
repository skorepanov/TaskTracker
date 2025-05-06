import React from 'react';
import { Checkbox } from 'antd';
import ITaskMovedToTrash from '../interfaces/ITaskMovedToTrash';

class TaskMovedToTrash extends React.Component<ITaskMovedToTrashProps> {
    render() {
        const task = this.props.task;

        const isCompleted = this.props.task.completedDateTime !== null;

        const description = task.description?.length > 0
            ? <><i>{task.description}</i><br /></>
            : null;

        const movedToTrashDateTime = task.movedToTrashDateTime
            ? <>Дата перемещения в корзину: {task.movedToTrashDateTime}</>
            : null;

        return (
            <div style={{ color: 'grey', marginBottom: 10 }}>
                <Checkbox
                    checked={isCompleted}
                    disabled={true}
                    style={{ marginRight: 5 }}
                />
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
