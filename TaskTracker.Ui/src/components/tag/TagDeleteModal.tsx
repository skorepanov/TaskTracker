import React from "react";
import { observer } from "mobx-react-lite";
import { Modal } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import ITag from "../../interfaces/ITag";

interface ITagDeleteModalProps {
    tag: ITag;
    isModalOpen: boolean;
    hideModal: () => void;
}

const TagDeleteModal: React.FC<ITagDeleteModalProps> = observer(props => {
    const rootStore = useStore();
    const t = useTranslation();

    const handleDeleteTagButtonClick = async () => {
        await rootStore.deleteTag(props.tag.id);
        props.hideModal();
    };

    const handleCancelClick = () => {
        props.hideModal();
    };

    return (
        <Modal
            title={t("deleteTag")}
            open={props.isModalOpen}
            okText={t("delete")}
            onOk={handleDeleteTagButtonClick}
            cancelText={t("cancel")}
            onCancel={handleCancelClick}>
            {t("deleteTag")} "{props.tag.title}"?
        </Modal>
    );
});

export default TagDeleteModal;
