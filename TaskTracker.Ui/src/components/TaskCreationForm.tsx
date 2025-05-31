import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import dayjs, { Dayjs } from "dayjs";
import { Input, Select, DatePicker, Button, Space } from "antd";
import { useStore } from "../stores/RootStore";

const { TextArea } = Input;

const TaskCreationForm: React.FC = observer(() => {
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [dueDateTime, setDueDateTime] = useState<Date | null>(null);
    const [folderId, setFolderId] = useState<number>(-1);

    const { taskStore, folderStore } = useStore();

    const inboxId = -1;

    const inboxOption = {
        key: inboxId,
        value: inboxId,
        label: "<Inbox>",
    };

    const folderOptions = folderStore.folders.map(f => ({
        key: f.id,
        value: f.id,
        label: f.title,
    }));

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleDescriptionChange = (
        event: React.ChangeEvent<HTMLTextAreaElement>
    ) => {
        setDescription(event.target.value);
    };

    const handleDueDateTimeChange = (date: Dayjs | null) => {
        setDueDateTime(date?.toDate() ?? null);
    };

    const handleFolderChange = (folderId: number) => {
        setFolderId(folderId);
    };

    const isCreateTaskButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleCreateTaskButtonClick = async () => {
        const normalizedFolderId = folderId === inboxId ? null : folderId;

        await taskStore.createTask(
            title,
            description,
            dueDateTime,
            normalizedFolderId
        );

        setTitle("");
        setDescription("");
        setDueDateTime(null);
        setFolderId(-1);
    };

    return (
        <Space direction="vertical">
            <strong>Новая задача</strong>
            <Input
                placeholder="Название задачи"
                value={title}
                onChange={handleTitleChange}
                style={{ width: 300 }}
            />
            <TextArea
                placeholder="Описание задачи"
                value={description}
                onChange={handleDescriptionChange}
                style={{ width: 300 }}
            />
            <DatePicker
                placeholder="Дата"
                value={dueDateTime !== null ? dayjs(dueDateTime) : null}
                onChange={handleDueDateTimeChange}
            />
            <Select
                placeholder="Папка"
                options={[inboxOption, ...folderOptions]}
                value={folderId}
                onChange={handleFolderChange}
                showSearch
                optionFilterProp="label"
                style={{ width: 300 }}
            />
            <Button
                onClick={handleCreateTaskButtonClick}
                disabled={isCreateTaskButtonDisabled()}>
                Добавить
            </Button>
        </Space>
    );
});

export default TaskCreationForm;
