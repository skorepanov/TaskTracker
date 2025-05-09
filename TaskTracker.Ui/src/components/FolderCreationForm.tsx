import React, { useState } from "react";
import { Input, Button, Space } from 'antd';

interface IFolderCreationFormProps {
    createFolder: (title: string) => Promise<void>;
}

const FolderCreationForm: React.FC<IFolderCreationFormProps> = (props) => {
    const [title, setTitle] = useState<string>('');

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    }

    const isCreateFolderButtonDisabled = () => {
        return title.trim() === '';
    }

    const handleCreateFolderButtonClick = async () => {
        await props.createFolder(title);
        setTitle('');
    }

    return (
        <Space direction='vertical'>
            <strong>Новая папка</strong>
            <Input
                placeholder='Название папки'
                value={title}
                onChange={handleTitleChange}
                style={{ width: 300 }}
            />
            <Button
                onClick={handleCreateFolderButtonClick}
                disabled={isCreateFolderButtonDisabled()}
            >
                Добавить
            </Button>
        </Space>
    );
}

export default FolderCreationForm;

