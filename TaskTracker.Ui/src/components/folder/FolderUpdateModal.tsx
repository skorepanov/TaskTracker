import React, { useState, useRef } from "react";
import { observer } from "mobx-react-lite";
import { Input, InputRef, Modal } from "antd";
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
    const titleRef = useRef<InputRef>(null);

    const handleAfterOpenChange = async (open: boolean) => {
        if (open && titleRef.current) {
            titleRef.current.focus();
        }
    }

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleTitlePressEnter
        = async (event: React.KeyboardEvent<HTMLInputElement>) => {
        event.stopPropagation();

        if (!isUpdateFolderButtonDisabled()) {
            await updateFolder();
        }
    }

    const isUpdateFolderButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleUpdateFolderButtonClick = async () => {
        await updateFolder();
    };

    const handleCancelClick = () => {
        setTitle(props.folder.title);
        props.hideModal();
    };

    const updateFolder = async () => {
        setTitle(title.trim());
        await folderStore.updateFolder(props.folder.id, title);
        props.hideModal();
    }

    return (
        <Modal
            title={t("updateFolder")}
            open={props.isModalOpen}
            afterOpenChange={handleAfterOpenChange}
            okText={t("save")}
            onOk={handleUpdateFolderButtonClick}
            okButtonProps={{
                disabled: isUpdateFolderButtonDisabled(),
            }}
            cancelText={t("cancel")}
            onCancel={handleCancelClick}>
            <Input
                ref={titleRef}
                placeholder={t("folderTitle")}
                value={title}
                onChange={handleTitleChange}
                onPressEnter={handleTitlePressEnter}
                onKeyDown={(event) => event.stopPropagation()}
                style={{ width: 300 }}
            />
        </Modal>
    );
});

export default FolderUpdateModal;
