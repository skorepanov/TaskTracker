import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { ColorPicker, Input, Modal, Space } from "antd";
import { useStore } from "../../stores/RootStore";
import { Color } from "antd/es/color-picker";
import ITag from "../../interfaces/ITag";

interface ITagUpdateModalProps {
    tag: ITag;
    isModalOpen: boolean;
    hideModal: () => void;
}

const TagUpdateModal: React.FC<ITagUpdateModalProps> = observer(props => {
    const [title, setTitle] = useState<string>(props.tag.title);
    const [color, setColor] = useState<string>(props.tag.color);

    const { tagStore } = useStore();

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleColorChange = (color: Color) => {
        setColor(color.toHex());
    };

    const isUpdateTagButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleUpdateTagButtonClick = async () => {
        await tagStore.updateTag(props.tag.id, title, color);
        setTitle(props.tag.title);
        setColor(props.tag.color);
        props.hideModal();
    };

    const handleCancelClick = () => {
        setTitle(props.tag.title);
        setColor(props.tag.color);
        props.hideModal();
    };

    return (
        <Modal
            title="Редактировать тег"
            open={props.isModalOpen}
            okText="Сохранить"
            onOk={handleUpdateTagButtonClick}
            okButtonProps={{
                disabled: isUpdateTagButtonDisabled(),
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

export default TagUpdateModal;
