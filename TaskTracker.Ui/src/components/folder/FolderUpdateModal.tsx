import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Input, Modal } from "antd";
import { useStore } from "../../stores/RootStore";
import IFolder from "../../interfaces/IFolder";

interface IFolderUpdateModalProps {
    folder: IFolder;
    isModalOpen: boolean;
    hideModal: () => void;
}

const FolderUpdateModal: React.FC<IFolderUpdateModalProps> = observer(props => {
    const [title, setTitle] = useState<string>(props.folder.title);

    const { folderStore } = useStore();

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const isUpdateFolderButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleUpdateFolderButtonClick = async () => {
        setTitle(title.trim());
        await folderStore.updateFolder(props.folder.id, title);
        props.hideModal();
    };

    const handleCancelClick = () => {
        setTitle(props.folder.title);
        props.hideModal();
    };

    return (
        <Modal
            title="Редактировать папку"
            open={props.isModalOpen}
            okText="Сохранить"
            onOk={handleUpdateFolderButtonClick}
            okButtonProps={{
                disabled: isUpdateFolderButtonDisabled(),
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
});

export default FolderUpdateModal;
