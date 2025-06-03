import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { ColorPicker, Input, Modal, Space } from "antd";
import { useStore } from "../../stores/RootStore";
import { Color } from "antd/es/color-picker";

interface ITagCreationModalProps {
    isModalOpen: boolean;
    hideModal: () => void;
}

const TagCreationModal: React.FC<ITagCreationModalProps> = observer(props => {
    const defaultColor = "ffffff";

    const [title, setTitle] = useState<string>("");
    const [color, setColor] = useState<string>(defaultColor);

    const { tagStore } = useStore();

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
            title="Новый тег"
            open={props.isModalOpen}
            okText="Создать тег"
            onOk={handleCreateTagButtonClick}
            okButtonProps={{
                disabled: isCreateTagButtonDisabled(),
            }}
            cancelText="Отмена"
            onCancel={handleCancelClick}>
            <Space direction="vertical">
                <Input
                    placeholder="Название тега"
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
