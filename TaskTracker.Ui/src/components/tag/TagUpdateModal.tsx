import React, { useState, useRef } from "react";
import { observer } from "mobx-react-lite";
import { ColorPicker, Input, InputRef, Modal, Space } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import { Color } from "antd/es/color-picker";
import ITag from "../../interfaces/ITag";

interface ITagUpdateModalProps {
    tag: ITag;
    isModalOpen: boolean;
    hideModal: () => void;
}

const TagUpdateModal: React.FC<ITagUpdateModalProps> = observer(props => {
    const { tagStore } = useStore();
    const t = useTranslation();

    const [title, setTitle] = useState<string>(props.tag.title);
    const titleRef = useRef<InputRef>(null);
    const [color, setColor] = useState<string>(props.tag.color);

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

        if (!isUpdateTagButtonDisabled()) {
            await updateTag();
        }
    }

    const handleTitleKeyDown
        = (event: React.KeyboardEvent<HTMLInputElement>) => {
        event.stopPropagation();

        if (event.key === 'Escape') {
            handleCancelClick();
        }
    };

    const handleColorChange = (color: Color) => {
        setColor(color.toHex());
    };

    const isUpdateTagButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleUpdateTagButtonClick = async () => {
        await updateTag();
    };

    const handleCancelClick = () => {
        setTitle(props.tag.title);
        setColor(props.tag.color);
        props.hideModal();
    };

    const updateTag = async () => {
        setTitle(title.trim());
        await tagStore.updateTag(props.tag.id, title, color);
        props.hideModal();
    }

    return (
        <Modal
            title={t("updateTag")}
            open={props.isModalOpen}
            afterOpenChange={handleAfterOpenChange}
            okText={t("save")}
            onOk={handleUpdateTagButtonClick}
            okButtonProps={{
                disabled: isUpdateTagButtonDisabled(),
            }}
            cancelText={t("cancel")}
            onCancel={handleCancelClick}>
            <Space direction="vertical">
                <Input
                    ref={titleRef}
                    placeholder={t("tagTitle")}
                    value={title}
                    onChange={handleTitleChange}
                    onPressEnter={handleTitlePressEnter}
                    onKeyDown={handleTitleKeyDown}
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
