import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import TaskTag from "../tag/TaskTag";
import { Checkbox, Divider, Typography } from "antd";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import { formatDateTime } from "../../utils";

const { Title } = Typography;

const TaskUpdatePanel: React.FC = observer(() => {
    const { taskStore, folderStore, tagStore } = useStore();

    const { currentTask: task } = taskStore;

    const [isCompleted, setIsCompleted] = useState<boolean>();
    const [title, setTitle] = useState<string>("");
    const [modifiedDateTime, setModifiedDateTime] = useState<Date | null>(null);

    useEffect(() => {
        setIsCompleted(task !== undefined && task.completedDateTime !== null);
        setTitle(task?.title ?? "");
        setModifiedDateTime(task?.modifiedDateTime ?? null);
    }, [task]);

    if (!task) {
        return null;
    }

    const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

    const taskTagComponents = taskTags
        ? taskTags.map(t => (
              <TaskTag
                  key={t.id}
                  tag={t}
              />
          ))
        : [];

    const folder =
        task.folderId !== null
            ? folderStore.folders.find(f => f.id === task.folderId)?.title
            : "<Inbox>";

    const folderComponent = (
        <>
            Папка: {folder}
            <br />
        </>
    );

    const movedToTrashDateTimeComponent =
        task.movedToTrashDateTime !== null ? (
            <>
                Дата перемещения в корзину:{" "}
                {formatDateTime(task.movedToTrashDateTime)}
                <br />
            </>
        ) : null;

    const completedDateTimeComponent = isCompleted ? (
        <>
            Выполнена: {formatDateTime(task.completedDateTime)}
            <br />
        </>
    ) : null;

    const dueDateTimeComponent =
        task.dueDateTime !== null ? (
            <>Срок выполнения: {formatDateTime(task.dueDateTime)}</>
        ) : null;

    const overdueDaysComponent =
        task.overdueDaysCount > 0 ? (
            <span style={{ color: isCompleted ? "" : "red" }}>
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
        modifiedDateTime !== null ? (
            <>
                <i>Изменена: {formatDateTime(modifiedDateTime)}</i>
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

    const handleTitleChange = async (newTitle: string) => {
        if (newTitle.trim() === "") {
            return;
        }

        await taskStore.updateTask(
            task.id,
            newTitle,
            task.description,
            task.folderId,
            task.dueDateTime
        );
    };

    return (
        <div style={{ padding: "10px" }}>
            <Checkbox
                checked={isCompleted}
                onChange={handleCompletedChange}
                style={{ marginRight: 5 }}
            />
            <Divider />
            <Title
                level={4}
                editable={{
                    onChange: handleTitleChange,
                    triggerType: ["text", "icon"],
                }}>
                {title}
            </Title>
            <br />
            <div style={{ paddingTop: 5 }}>{taskTagComponents}</div>
            {taskTagComponents.length > 0 ? <br /> : null}
            <Divider />
            {folderComponent}
            <Divider />
            {task.description}
            <Divider />
            {movedToTrashDateTimeComponent}
            {completedDateTimeComponent}
            {overdueComponent}
            {modifiedDateTimeComponent}
            <i>Создана: {formatDateTime(task.createdDateTime)}</i>
        </div>
    );
});

export default TaskUpdatePanel;
