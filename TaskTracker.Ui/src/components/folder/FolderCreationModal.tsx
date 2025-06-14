import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Input, Modal } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";

interface IFolderCreationModalProps {
    isModalOpen: boolean;
    hideModal: () => void;
}

const FolderCreationModal: React.FC<IFolderCreationModalProps> = observer(
    props => {
        const { folderStore } = useStore();
        const t = useTranslation();

        const [title, setTitle] = useState<string>("");

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
                title={t("newFolder")}
                open={props.isModalOpen}
                okText={t("createFolder")}
                onOk={handleCreateFolderButtonClick}
                okButtonProps={{
                    disabled: isCreateFolderButtonDisabled(),
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
    }
);

export default FolderCreationModal;
