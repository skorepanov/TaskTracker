import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import { Checkbox, DatePicker, Divider, Input, Select, Typography } from "antd";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import dayjs, { Dayjs } from "dayjs";
import { formatDateTime } from "../../utils";

const { Title } = Typography;
const { TextArea } = Input;

const TaskUpdatePanel: React.FC = observer(() => {
    const { taskStore, folderStore, tagStore } = useStore();

    const { currentTask: task } = taskStore;

    if (!task) {
        return null;
    }

    const isDisabled = task.movedToTrashDateTime !== null;

    const isCompleted = task.completedDateTime !== null;

    const inboxId = -1;

    const inboxOption = {
        key: inboxId,
        value: inboxId,
        label: "<Inbox>",
    };

    const folderOptions = folderStore.getSortedFolders().map(f => ({
        key: f.id,
        value: f.id,
        label: f.title,
    }));

    const dueDateTime =
        task.dueDateTime !== null ? dayjs(task.dueDateTime) : null;

    const dueDateTimeColor = task.overdueDaysCount > 0 ? "red" : "";

    const allTagOptions = tagStore.tags.map(t => ({
        key: t.id,
        value: t.id,
        label: t.title,
    }));

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

    const modifiedDateTimeComponent =
        task.modifiedDateTime !== null ? (
            <>
                Изменена: {formatDateTime(task.modifiedDateTime)}
                <br />
            </>
        ) : null;

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

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
            task.dueDateTime,
            task.tagIds
        );
    };

    const handleDescriptionChange = async (
        event: React.ChangeEvent<HTMLTextAreaElement>
    ) => {
        await taskStore.updateTask(
            task.id,
            task.title,
            event.target.value,
            task.folderId,
            task.dueDateTime,
            task.tagIds
        );
    };

    const handleFolderChange = async (newFolderId: number) => {
        const normalizedFolderId = newFolderId === inboxId ? null : newFolderId;

        await taskStore.updateTask(
            task.id,
            task.title,
            task.description,
            normalizedFolderId,
            task.dueDateTime,
            task.tagIds
        );
    };

    const handleDueDateTimeChange = async (newDueDateTime: Dayjs | null) => {
        const normalizedDueDateTime = newDueDateTime?.toDate() ?? null;

        await taskStore.updateTask(
            task.id,
            task.title,
            task.description,
            task.folderId,
            normalizedDueDateTime,
            task.tagIds
        );
    };

    const handleTagChange = async (newTagIds: number[]) => {
        await taskStore.updateTask(
            task.id,
            task.title,
            task.description,
            task.folderId,
            task.dueDateTime,
            newTagIds
        );
    };

    const folderComponent = !isDisabled ? (
        <Select
            placeholder="Папка"
            options={[inboxOption, ...folderOptions]}
            value={task.folderId ?? inboxId}
            onChange={handleFolderChange}
            showSearch
            optionFilterProp="label"
            style={{ width: 200 }}
            popupMatchSelectWidth={false}
        />
    ) : null;

    return (
        <div style={{ padding: "10px" }}>
            <Checkbox
                checked={isCompleted}
                disabled={isDisabled}
                onChange={handleCompletedChange}
            />
            <DatePicker
                placeholder="Когда выполнить"
                value={dueDateTime}
                disabled={isDisabled}
                onChange={handleDueDateTimeChange}
                style={{ color: dueDateTimeColor, width: 180, marginLeft: 10 }}
            />
            <Divider size="small" />
            <Title
                level={4}
                disabled={isDisabled}
                editable={{
                    triggerType: !isDisabled ? ["text", "icon"] : [],
                    onChange: handleTitleChange,
                }}
                style={{
                    marginTop: 15,
                    marginBottom: 15,
                }}>
                {task.title}
            </Title>
            <Select
                mode="multiple"
                placeholder="Теги"
                options={allTagOptions}
                value={task.tagIds}
                showSearch
                optionFilterProp="label"
                disabled={isDisabled}
                onChange={handleTagChange}
                style={{ width: "100%" }}></Select>
            <TextArea
                placeholder="Описание задачи"
                value={task.description ?? ""}
                disabled={isDisabled}
                onChange={handleDescriptionChange}
                style={{ width: "100%", height: 400, marginTop: 10 }}
            />
            <br />
            <br />
            {folderComponent}
            <Divider size="small" />
            {movedToTrashDateTimeComponent}
            {completedDateTimeComponent}
            {modifiedDateTimeComponent}
            Создана: {formatDateTime(task.createdDateTime)}
        </div>
    );
});

export default TaskUpdatePanel;
