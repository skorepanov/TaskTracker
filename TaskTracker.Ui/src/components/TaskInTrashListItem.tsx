import React from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Button } from "antd";
import { useStore } from "../stores/RootStore";
import { formatDateTime } from "../utils";
import ITask from "../interfaces/ITask";

interface ITaskInTrashListItemProps {
    task: ITask;
}

const TaskInTrashListItem: React.FC<ITaskInTrashListItemProps> = observer(
    props => {
        const { taskStore } = useStore();

        const { task } = props;

        const isCompleted = task.completedDateTime !== null;

        const descriptionComponent =
            task.description !== null && task.description.length > 0 ? (
                <i>{task.description}</i>
            ) : null;

        const movedToTrashDateTimeComponent =
            task.movedToTrashDateTime !== null ? (
                <>
                    Дата перемещения в корзину:{" "}
                    {formatDateTime(task.movedToTrashDateTime)}
                </>
            ) : null;

        const modifiedDateTimeComponent =
            task.modifiedDateTime !== null ? (
                <>
                    <i>Изменена: {formatDateTime(task.modifiedDateTime)}</i>
                    <br />
                </>
            ) : null;

        const handleRestoreTaskButtonClick = async () => {
            await taskStore.moveTaskFromTrash(task);
        };

        const handleDeleteTaskButtonClick = async () => {
            await taskStore.deleteTask(task);
        };

        return (
            <div style={{ color: "grey", marginBottom: 10 }}>
                <Checkbox
                    checked={isCompleted}
                    disabled={true}
                    style={{ marginRight: 5 }}
                />
                {task.title}
                <br />
                {descriptionComponent}
                <br />
                {movedToTrashDateTimeComponent}
                <br />
                {modifiedDateTimeComponent}
                <i>Создана: {formatDateTime(task.createdDateTime)}</i>
                <br />
                <Button onClick={handleRestoreTaskButtonClick}>
                    Восстановить
                </Button>
                <Button onClick={handleDeleteTaskButtonClick}>Удалить</Button>
            </div>
        );
    }
);

export default TaskInTrashListItem;
