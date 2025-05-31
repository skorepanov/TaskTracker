import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Button } from "antd";
import { useStore } from "../stores/RootStore";
import { formatDateTime } from "../utils";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import ITask from "../interfaces/ITask";

interface ITaskListItemProps {
    task: ITask;
}

const TaskListItem: React.FC<ITaskListItemProps> = observer(props => {
    const { taskStore } = useStore();

    const { task } = props;
    const isTaskCompleted = task.completedDateTime !== null;

    const [isCompleted, setIsCompleted] = useState<boolean>(isTaskCompleted);

    const textColor = isTaskCompleted ? "green" : "black";

    const descriptionComponent = (
        <>
            <i>{task.description}</i>
            <br />
        </>
    );

    const completedDateTimeComponent = isTaskCompleted ? (
        <>
            Выполнено: {formatDateTime(task.completedDateTime)}
            <br />
        </>
    ) : null;

    const dueDateTimeComponent =
        task.dueDateTime !== null ? (
            <>Срок выполнения: {formatDateTime(task.dueDateTime)}</>
        ) : null;

    const overdueDaysComponent =
        task.overdueDaysCount > 0 ? (
            <span style={{ color: isTaskCompleted ? "" : "red" }}>
                ({task.overdueDaysCount} дней назад)
            </span>
        ) : null;

    const overdueComponent =
        task.dueDateTime !== null ? (
            <>
                {dueDateTimeComponent} {overdueDaysComponent}
                <br />
            </>
        ) : null;

    const modifiedDateTimeComponent =
        task.modifiedDateTime !== null ? (
            <>
                <i>Изменена: {formatDateTime(task.modifiedDateTime)}</i>
                <br />
            </>
        ) : null;

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        setIsCompleted(isCompletedNew);

        if (isCompletedNew) {
            await taskStore.completeTask(task);
        } else {
            await taskStore.incompleteTask(task);
        }
    };

    const handleMoveToTrashButtonClick = async () => {
        await taskStore.moveTaskToTrash(task);
    };

    return (
        <div style={{ color: textColor, marginBottom: 10 }}>
            <Checkbox
                checked={isCompleted}
                onChange={handleCompletedChange}
                style={{ marginRight: 5 }}
            />
            {task.title}
            <br />
            {descriptionComponent}
            {completedDateTimeComponent}
            {overdueComponent}
            {modifiedDateTimeComponent}
            <i>Создана: {formatDateTime(task.createdDateTime)}</i>
            <br />
            <Button onClick={handleMoveToTrashButtonClick}>В корзину</Button>
        </div>
    );
});

export default TaskListItem;
