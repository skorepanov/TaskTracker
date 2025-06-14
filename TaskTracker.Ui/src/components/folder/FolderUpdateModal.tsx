import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Input, Modal } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import IFolder from "../../interfaces/IFolder";

interface IFolderUpdateModalProps {
    folder: IFolder;
    isModalOpen: boolean;
    hideModal: () => void;
}

const FolderUpdateModal: React.FC<IFolderUpdateModalProps> = observer(props => {
    const { folderStore } = useStore();
    const t = useTranslation();

    const [title, setTitle] = useState<string>(props.folder.title);

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
            title={t("updateFolder")}
            open={props.isModalOpen}
            okText={t("save")}
            onOk={handleUpdateFolderButtonClick}
            okButtonProps={{
                disabled: isUpdateFolderButtonDisabled(),
            }}
            cancelText={t("cancel")}
            onCancel={handleCancelClick}>
            <Input
                placeholder={t("folderTitle")}
                value={title}
                onChange={handleTitleChange}
                style={{ width: 300 }}
            />
        </Modal>
    );
});

export default FolderUpdateModal;
