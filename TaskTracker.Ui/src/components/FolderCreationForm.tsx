import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Input, Button, Space } from "antd";
import { useStore } from "../stores/RootStore";

const FolderCreationForm: React.FC = observer(() => {
    const [title, setTitle] = useState<string>("");

    const { folderStore } = useStore();

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    }

    const isCreateFolderButtonDisabled = () => {
        return title.trim() === "";
    }

    const handleCreateFolderButtonClick = async () => {
        await folderStore.createFolder(title);
        setTitle("");
    }

    return (
        <Space direction="vertical">
            <strong>Новая папка</strong>
            <Input
                placeholder="Название папки"
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
});

export default FolderCreationForm;

