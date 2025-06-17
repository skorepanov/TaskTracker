import React from "react";
import { observer } from "mobx-react-lite";
import { Modal } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import IFolder from "../../interfaces/IFolder";

interface IFolderDeleteModalProps {
    folder: IFolder;
    isModalOpen: boolean;
    hideModal: () => void;
}

const FolderDeleteModal: React.FC<IFolderDeleteModalProps> = observer(props => {
    const { folderStore } = useStore();
    const t = useTranslation();

    const handleDeleteFolderButtonClick = async () => {
        await folderStore.deleteFolder(props.folder);
        props.hideModal();
    };

    const handleCancelClick = () => {
        props.hideModal();
    };

    return (
        <Modal
            title={t("deleteFolder")}
            open={props.isModalOpen}
            okText={t("delete")}
            onOk={handleDeleteFolderButtonClick}
            cancelText={t("cancel")}
            onCancel={handleCancelClick}>
            {t("deleteFolder")} "{props.folder.title}"?
        </Modal>
    );
});

export default FolderDeleteModal;
