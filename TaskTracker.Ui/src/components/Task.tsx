import React, { useState } from 'react';
import { Checkbox, Button } from 'antd';
import type { CheckboxChangeEvent } from 'antd/es/checkbox';
import ITask from '../interfaces/ITask';

interface ITaskProps {
    task: ITask;
    completeTask: (task: ITask) => Promise<void>;
    incompleteTask: (task: ITask) => Promise<void>;
    moveTaskToTrash: (task: ITask) => Promise<void>;
}

const Task: React.FC<ITaskProps> = (props) => {
    const { task } = props;
    const isTaskCompleted = task.completedDateTime !== null;

    const [isCompleted, setIsCompleted] = useState<boolean>(isTaskCompleted);

    const textColor = isTaskCompleted
        ? 'green'
        : 'black';

    const descriptionAsText = <><i>{task.description}</i><br /></>;

    const completedDateTimeAsText = isTaskCompleted
        ? <>Выполнено: {task.completedDateTime}<br /></>
        : null;

    const dueDateTimeAsText = task.dueDateTime !== null
        ? <>Срок выполнения: {task.dueDateTime}</>
        : null;

    const overdueDaysAsText = task.overdueDaysCount > 0
        ? <>({task.overdueDaysCount} дней назад)</>
        : null;

    const overdueAsText = task.dueDateTime !== null
        ? <>{dueDateTimeAsText} {overdueDaysAsText}<br /></>
        : null;

    const modifiedDateTimeAsText = task.modifiedDateTime !== null
        ? <><i>Изменена: {task.modifiedDateTime.toString()}</i><br /></>
        : null;

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        setIsCompleted(isCompletedNew);

        if (isCompletedNew) {
            await props.completeTask(task);
        } else {
            await props.incompleteTask(task);
        }
    }

    const handleMoveToTrashButtonClick = async () => {
        await props.moveTaskToTrash(task);
    }

    return (
        <div style={{ color: textColor, marginBottom: 10 }}>
            <Checkbox
                checked={isCompleted}
                onChange={handleCompletedChange}
                style={{ marginRight: 5 }}
            />
            [{task.id}] {task.title}<br />
            {descriptionAsText}
            {completedDateTimeAsText}
            {overdueAsText}
            {modifiedDateTimeAsText}
            <i>Создана: {task.createdDateTime.toString()}</i><br />
            <Button
                onClick={handleMoveToTrashButtonClick}
            >
                В корзину
            </Button>
        </div>
    );
}

export default Task;
