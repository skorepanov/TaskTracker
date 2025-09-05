import React, { useState, useRef } from "react";
import { observer } from "mobx-react-lite";
import { Input, InputRef, Modal } from "antd";
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
        const titleRef = useRef<InputRef>(null);

        const handleAfterOpenChange = async (open: boolean) => {
            if (open && titleRef.current) {
                titleRef.current.focus();
            }
        }

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
                afterOpenChange={handleAfterOpenChange}
                okText={t("createFolder")}
                onOk={handleCreateFolderButtonClick}
                okButtonProps={{
                    disabled: isCreateFolderButtonDisabled(),
                }}
                cancelText={t("cancel")}
                onCancel={handleCancelClick}>
                <Input
                    ref={titleRef}
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
