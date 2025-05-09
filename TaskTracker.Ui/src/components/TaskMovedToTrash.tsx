import React from 'react';
import { Checkbox } from 'antd';
import ITaskMovedToTrash from '../interfaces/ITaskMovedToTrash';

interface ITaskMovedToTrashProps {
    task: ITaskMovedToTrash;
}

const TaskMovedToTrash: React.FC<ITaskMovedToTrashProps> = ({ task }) => {
    const isCompleted = task.completedDateTime !== null;

    const description = task.description?.length > 0
        ? <i>{task.description}</i>
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
            {description}<br />
            {movedToTrashDateTime}
        </div>
    );
}

export default TaskMovedToTrash;
