import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { ColorPicker, Input, Modal, Space } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import { Color } from "antd/es/color-picker";

interface ITagCreationModalProps {
    isModalOpen: boolean;
    hideModal: () => void;
}

const TagCreationModal: React.FC<ITagCreationModalProps> = observer(props => {
    const { tagStore } = useStore();
    const t = useTranslation();

    const defaultColor = "ffffff";

    const [title, setTitle] = useState<string>("");
    const [color, setColor] = useState<string>(defaultColor);

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleColorChange = (color: Color) => {
        setColor(color.toHex());
    };

    const isCreateTagButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleCreateTagButtonClick = async () => {
        await tagStore.createTag(title, color);
        setTitle("");
        setColor(defaultColor);
        props.hideModal();
    };

    const handleCancelClick = () => {
        setTitle("");
        setColor(defaultColor);
        props.hideModal();
    };

    return (
        <Modal
            title={t("newTag")}
            open={props.isModalOpen}
            okText={t("createTag")}
            onOk={handleCreateTagButtonClick}
            okButtonProps={{
                disabled: isCreateTagButtonDisabled(),
            }}
            cancelText={t("cancel")}
            onCancel={handleCancelClick}>
            <Space direction="vertical">
                <Input
                    placeholder={t("tagTitle")}
                    value={title}
                    onChange={handleTitleChange}
                    style={{ width: "300" }}
                />
                <ColorPicker
                    value={color}
                    onChange={handleColorChange}
                />
            </Space>
        </Modal>
    );
});

export default TagCreationModal;
