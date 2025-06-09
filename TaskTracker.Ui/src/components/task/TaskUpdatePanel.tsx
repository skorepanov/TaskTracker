import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import TaskTag from "../tag/TaskTag";
import { Checkbox, DatePicker, Divider, Input, Select, Typography } from "antd";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import dayjs, { Dayjs } from "dayjs";
import { formatDateTime } from "../../utils";

const { Title } = Typography;
const { TextArea } = Input;

const TaskUpdatePanel: React.FC = observer(() => {
    const inboxId = -1;

    const { taskStore, folderStore, tagStore } = useStore();

    const { currentTask: task } = taskStore;

    const [isCompleted, setIsCompleted] = useState<boolean>();
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [folderId, setFolderId] = useState<number | null>(null);
    const [dueDateTime, setDueDateTime] = useState<Date | null>();
    const [modifiedDateTime, setModifiedDateTime] = useState<Date | null>(null);

    useEffect(() => {
        if (!task) {
            return;
        }

        setIsCompleted(task.completedDateTime !== null);
        setTitle(task.title);
        setDescription(task.description ?? "");
        setFolderId(task.folderId ?? inboxId);
        setDueDateTime(task.dueDateTime);
        setModifiedDateTime(task.modifiedDateTime);
    }, [task, inboxId]);

    if (!task) {
        return null;
    }

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

    const dueDateTimeColor = task.overdueDaysCount > 0 ? "red" : "";

    const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

    const taskTagComponents = taskTags
        ? taskTags.map(t => (
              <TaskTag
                  key={t.id}
                  tag={t}
              />
          ))
        : [];

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
        modifiedDateTime !== null ? (
            <>
                Изменена: {formatDateTime(modifiedDateTime)}
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

    const handleDescriptionChange = async (
        event: React.ChangeEvent<HTMLTextAreaElement>
    ) => {
        await taskStore.updateTask(
            task.id,
            task.title,
            event.target.value,
            task.folderId,
            task.dueDateTime
        );
    };

    const handleFolderChange = async (newFolderId: number) => {
        const normalizedFolderId = newFolderId === inboxId ? null : newFolderId;

        await taskStore.updateTask(
            task.id,
            task.title,
            task.description,
            normalizedFolderId,
            task.dueDateTime
        );
    };

    const handleDueDateTimeChange = async (newDueDateTime: Dayjs | null) => {
        const normalizedDueDateTime = newDueDateTime?.toDate() ?? null;

        await taskStore.updateTask(
            task.id,
            task.title,
            task.description,
            task.folderId,
            normalizedDueDateTime
        );
    };

    return (
        <div style={{ padding: "10px" }}>
            <Checkbox
                checked={isCompleted}
                onChange={handleCompletedChange}
            />
            <DatePicker
                placeholder="Когда выполнить"
                value={dueDateTime !== null ? dayjs(dueDateTime) : null}
                onChange={handleDueDateTimeChange}
                style={{ color: dueDateTimeColor, width: 180, marginLeft: 10 }}
            />
            <Divider size="small" />
            <Title
                level={4}
                editable={{
                    onChange: handleTitleChange,
                    triggerType: ["text", "icon"],
                }}
                style={{
                    marginTop: 15,
                    marginBottom: 15,
                }}>
                {title}
            </Title>
            <div style={{ paddingTop: 5 }}>{taskTagComponents}</div>
            <Divider size="small" />
            <TextArea
                placeholder="Описание задачи"
                value={description}
                onChange={handleDescriptionChange}
                style={{ width: "100%", height: 400 }}
            />
            <br />
            <br />
            <Select
                placeholder="Папка"
                options={[inboxOption, ...folderOptions]}
                value={folderId}
                onChange={handleFolderChange}
                showSearch
                optionFilterProp="label"
                style={{ width: 200 }}
                popupMatchSelectWidth={false}
            />
            <Divider size="small" />
            {movedToTrashDateTimeComponent}
            {completedDateTimeComponent}
            {modifiedDateTimeComponent}
            Создана: {formatDateTime(task.createdDateTime)}
        </div>
    );
});

export default TaskUpdatePanel;
