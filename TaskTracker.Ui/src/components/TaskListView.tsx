import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Button } from "antd";
import { useStore } from "../stores/RootStore";
import { formatDateTime } from "../utils";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import ITask from "../interfaces/ITask";

interface ITaskListViewProps {
    task: ITask;
}

const TaskListView: React.FC<ITaskListViewProps> = observer(props => {
    const { taskStore } = useStore();

    const { task } = props;
    const isTaskCompleted = task.completedDateTime !== null;

    const [isCompleted, setIsCompleted] = useState<boolean>(isTaskCompleted);

    const textColor = isTaskCompleted ? "green" : "black";

    const descriptionAsText = (
        <>
            <i>{task.description}</i>
            <br />
        </>
    );

    const completedDateTimeAsText = isTaskCompleted ? (
        <>
            Выполнено: {formatDateTime(task.completedDateTime)}
            <br />
        </>
    ) : null;

    const dueDateTimeAsText =
        task.dueDateTime !== null ? (
            <>Срок выполнения: {formatDateTime(task.dueDateTime)}</>
        ) : null;

    const overdueDaysAsText =
        task.overdueDaysCount > 0 ? (
            <>({task.overdueDaysCount} дней назад)</>
        ) : null;

    const overdueAsText =
        task.dueDateTime !== null ? (
            <>
                {dueDateTimeAsText} {overdueDaysAsText}
                <br />
            </>
        ) : null;

    const modifiedDateTimeAsText =
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
            [{task.id}] {task.title}
            <br />
            {descriptionAsText}
            {completedDateTimeAsText}
            {overdueAsText}
            {modifiedDateTimeAsText}
            <i>Создана: {formatDateTime(task.createdDateTime)}</i>
            <br />
            <Button onClick={handleMoveToTrashButtonClick}>В корзину</Button>
        </div>
    );
});

export default TaskListView;
