import React, { useState } from 'react';
import dayjs, { Dayjs } from 'dayjs';
import { Input, Select, DatePicker, Button, Space } from 'antd';
import IFolder from '../interfaces/IFolder';

const { TextArea } = Input;
const { Option } = Select;

interface ITaskCreationFormProps {
    folders: IFolder[];
    createTask: (
        title: string,
        description: string,
        dueDateTime: Date | null,
        folderId: number | null
    ) => Promise<void>;
}

const TaskCreationForm: React.FC<ITaskCreationFormProps> = (props) => {
    const [title, setTitle] = useState<string>('');
    const [description, setDescription] = useState<string>('');
    const [dueDateTime, setDueDateTime] = useState<Date | null>(null);
    const [folderId, setFolderId] = useState<number | null>(null);

    const dayjsDueDateTime = dueDateTime !== null
        ? dayjs(dueDateTime)
        : null;

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    }

    const handleDescriptionChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setDescription(event.target.value);
    }

    const handleDueDateTimeChange = (date: Dayjs | null) => {
        setDueDateTime(date?.toDate() ?? null);
    }

    const handleFolderChange = (folderId: number) => {
        setFolderId(folderId);
    }

    const isCreateTaskButtonDisabled = () => {
        return title.trim() === '';
    }

    const handleCreateTaskButtonClick = async () => {
        await props.createTask(title, description, dueDateTime, folderId);

        setTitle('');
        setDescription('');
        setDueDateTime(null);
        setFolderId(null);
    }

    return (
        <Space direction='vertical'>
            <strong>Новая задача</strong>
            <Input
                placeholder='Название задачи'
                value={title}
                onChange={handleTitleChange}
                style={{ width: 300 }}
            />
            <TextArea
                placeholder='Описание задачи'
                value={description}
                onChange={handleDescriptionChange}
                style={{ width: 300 }}
            />
            <DatePicker
                placeholder='Дата'
                defaultValue={dayjsDueDateTime}
                onChange={handleDueDateTimeChange}
            />
            <Select
                placeholder='Папка'
                onChange={handleFolderChange}
                style={{ width: 300 }}
            >
                {
                    props.folders.map(f =>
                        <Option
                            key={f.id}
                            value={f.id}
                        >
                            {f.title}
                        </Option>
                    )
                }
            </Select>
            <Button
                onClick={handleCreateTaskButtonClick}
                disabled={isCreateTaskButtonDisabled()}
            >Добавить</Button>
        </Space>
    );
}

export default TaskCreationForm;
