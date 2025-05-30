import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Input, Modal } from "antd";
import { useStore } from "../stores/RootStore";

interface IFolderCreationModalProps {
    isModalOpen: boolean;
    hideModal: () => void;
}

const FolderCreationModal: React.FC<IFolderCreationModalProps> = observer(
    props => {
        const [title, setTitle] = useState<string>("");

        const { folderStore } = useStore();

        const handleTitleChange = (
            event: React.ChangeEvent<HTMLInputElement>
        ) => {
            setTitle(event.target.value);
        };

        const isCreateFolderButtonDisabled = () => {
            return title.trim() === "";
        };

        const handleCreateFolderButtonClick = async () => {
            await folderStore.createFolder(title);
            setTitle("");
            props.hideModal();
        };

        const handleCancelClick = () => {
            setTitle("");
            props.hideModal();
        };

        return (
            <Modal
                title="Новая папка"
                open={props.isModalOpen}
                okText="Создать папку"
                onOk={handleCreateFolderButtonClick}
                okButtonProps={{
                    disabled: isCreateFolderButtonDisabled(),
                }}
                cancelText="Отмена"
                onCancel={handleCancelClick}>
                <Input
                    placeholder="Название папки"
                    value={title}
                    onChange={handleTitleChange}
                    style={{ width: 300 }}
                />
            </Modal>
        );
    }
);

export default FolderCreationModal;
