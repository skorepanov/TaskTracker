import React from 'react';
import { Checkbox, Button } from 'antd';
import { formatDateTime } from '../utils';
import ITask from "../interfaces/ITask";

interface ITaskMovedToTrashProps {
    task: ITask;
    moveTaskFromTrash: (task: ITask) => Promise<void>;
    deleteTask: (task: ITask) => Promise<void>;
}

const TaskMovedToTrash: React.FC<ITaskMovedToTrashProps> = (props) => {
    const { task } = props;

    const isCompleted = task.completedDateTime !== null;

    const description = task.description !== null && task.description.length > 0
        ? <i>{task.description}</i>
        : null;

    const movedToTrashDateTime = task.movedToTrashDateTime !== null
        ? <>Дата перемещения в корзину: {formatDateTime(task.movedToTrashDateTime)}</>
        : null;

    const modifiedDateTimeAsText = task.modifiedDateTime !== null
        ? <><i>Изменена: {formatDateTime(task.modifiedDateTime)}</i><br /></>
        : null;

    const handleRestoreTaskButtonClick = async () => {
        await props.moveTaskFromTrash(task);
    }

    const handleDeleteTaskButtonClick = async () => {
        await props.deleteTask(task);
    }

    return (
        <div style={{ color: 'grey', marginBottom: 10 }}>
            <Checkbox
                checked={isCompleted}
                disabled={true}
                style={{ marginRight: 5 }}
            />
            [{task.id}] {task.title}<br />
            {description}<br />
            {movedToTrashDateTime}<br />
            {modifiedDateTimeAsText}
            <i>Создана: {formatDateTime(task.createdDateTime)}</i><br />
            <Button
                onClick={handleRestoreTaskButtonClick}
            >
                Восстановить
            </Button>
            <Button
                onClick={handleDeleteTaskButtonClick}
            >
                Удалить
            </Button>
        </div>
    );
}

export default TaskMovedToTrash;
