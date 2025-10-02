import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import {
    Checkbox,
    DatePicker,
    Divider,
    Input,
    Select,
    SelectProps,
    Tag,
    Typography,
} from "antd";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import dayjs, { Dayjs } from "dayjs";
import { useTranslation } from "../../hooks/useTranslation";
import { useDebounce } from "../../hooks/useDebounce";
import { dateFormat, formatDateTime } from "../../utils/dateTimeFormatter";

type TagRender = SelectProps["tagRender"];

const { Title } = Typography;
const { TextArea } = Input;

const TaskUpdatePanel: React.FC = observer(() => {
    const { taskStore, folderStore, tagStore } = useStore();
    const t = useTranslation();

    const { currentTask: task } = taskStore;

    const [description, setDecription] = useState<string>(task?.description ?? "");

    const updateTaskDebounced = useDebounce(async (
        taskId: number,
        title: string,
        description: string,
        folderId: number | null,
        dueDateTime: Date | null,
        tagIds: number[] | null
    ) => {
        await taskStore.updateTask(
            taskId,
            title,
            description,
            folderId,
            dueDateTime,
            tagIds
        );
    }, 5000);

    useEffect(() => {
        setDecription(task?.description ?? "");
    }, [task?.description]);

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
        color: t.color,
    }));

    const movedToTrashDateTimeComponent =
        task.movedToTrashDateTime !== null ? (
            <div>
                {t("taskMovedToTrash")}:{" "}
                {formatDateTime(task.movedToTrashDateTime)}
            </div>
        ) : null;

    const completedDateTimeComponent = isCompleted ? (
        <div>
            {t("taskCompleted")}: {formatDateTime(task.completedDateTime)}
        </div>
    ) : null;

    const modifiedDateTimeComponent =
        task.modifiedDateTime !== null ? (
            <div>
                {t("taskUpdated")}: {formatDateTime(task.modifiedDateTime)}
            </div>
        ) : null;

    const createdDateTimeComponent = (
        <div>
            {t("taskCreated")}: {formatDateTime(task.createdDateTime)}
        </div>
    );

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        if (isCompletedNew) {
            await taskStore.completeTask(task.id);
        } else {
            await taskStore.incompleteTask(task.id);
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
        setDecription(event.target.value);

        updateTaskDebounced(
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

    const tagRender: TagRender = props => {
        const { value: key, label, closable, onClose } = props;

        const handleMouseDown = (event: React.MouseEvent<HTMLSpanElement>) => {
            event.preventDefault();
            event.stopPropagation();
        };

        const color = allTagOptions.find(t => t.key === key)?.color;

        return (
            <Tag
                color={`#${color}`}
                closable={closable}
                onClose={onClose}
                onMouseDown={handleMouseDown}>
                <div
                    style={{
                        display: "inline-block",
                        mixBlendMode: "difference",
                    }}>
                    {label}
                </div>
            </Tag>
        );
    };

    const folderComponent = !isDisabled ? (
        <Select
            placeholder={t("folder")}
            options={[inboxOption, ...folderOptions]}
            value={task.folderId ?? inboxId}
            onChange={handleFolderChange}
            showSearch
            optionFilterProp="label"
            popupMatchSelectWidth={false}
            style={{
                width: 200,
                marginLeft: 10,
                marginTop: 10,
            }}
        />
    ) : null;

    return (
        <div
            style={{
                display: "flex",
                flexDirection: "column",
                height: "100%",
            }}>
            <div style={{ marginLeft: 10, marginTop: 10 }}>
                <Checkbox
                    checked={isCompleted}
                    disabled={isDisabled}
                    onChange={handleCompletedChange}
                />
                <DatePicker
                    placeholder={t("taskDueDateTime")}
                    value={dueDateTime}
                    format={dateFormat}
                    disabled={isDisabled}
                    onChange={handleDueDateTimeChange}
                    style={{
                        color: dueDateTimeColor,
                        width: 180,
                        marginLeft: 10,
                    }}
                />
            </div>
            <Divider size="small" />
            <Title
                level={4}
                disabled={isDisabled}
                editable={{
                    triggerType: !isDisabled ? ["text"] : [],
                    onChange: handleTitleChange,
                }}
                style={{ marginLeft: 12, marginTop: 0 }}>
                {task.title}
            </Title>
            <Select
                mode="multiple"
                tagRender={tagRender}
                placeholder={t("tags")}
                options={allTagOptions}
                value={task.tagIds}
                showSearch
                optionFilterProp="label"
                disabled={isDisabled}
                onChange={handleTagChange}
                style={{ marginLeft: 10, marginRight: 10, marginBottom: 10 }}
            />
            <TextArea
                placeholder={t("taskDescription")}
                value={description}
                disabled={isDisabled}
                variant="borderless"
                onChange={handleDescriptionChange}
                style={{ flexGrow: 1, height: "100%", resize: "none" }}
            />
            {folderComponent}
            <Divider size="small" />
            <div style={{ marginLeft: 10, marginBottom: 10 }}>
                {movedToTrashDateTimeComponent}
                {completedDateTimeComponent}
                {modifiedDateTimeComponent}
                {createdDateTimeComponent}
            </div>
        </div>
    );
});

export default TaskUpdatePanel;
