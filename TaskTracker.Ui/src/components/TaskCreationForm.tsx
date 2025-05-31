import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import dayjs, { Dayjs } from "dayjs";
import { Input, Select, DatePicker, Button, Space } from "antd";
import { useStore } from "../stores/RootStore";

const { TextArea } = Input;
const { Option } = Select;

const TaskCreationForm: React.FC = observer(() => {
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [dueDateTime, setDueDateTime] = useState<Date | null>(null);
    const [folderId, setFolderId] = useState<number | null>(null);

    const { taskStore, folderStore } = useStore();

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
        await taskStore.createTask(title, description, dueDateTime, folderId);

        setTitle("");
        setDescription("");
        setDueDateTime(null);
        setFolderId(null);
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
                onChange={handleFolderChange}
                style={{ width: 300 }}>
                {folderStore.folders.map(f => (
                    <Option
                        key={f.id}
                        value={f.id}>
                        {f.title}
                    </Option>
                ))}
            </Select>
            <Button
                onClick={handleCreateTaskButtonClick}
                disabled={isCreateTaskButtonDisabled()}>
                Добавить
            </Button>
        </Space>
    );
});

export default TaskCreationForm;
