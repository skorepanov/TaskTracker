import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { DatePicker, Input, Modal, Select, Space } from "antd";
import { useStore } from "../stores/RootStore";
import dayjs, { Dayjs } from "dayjs";

const { TextArea } = Input;

interface ITaskCreationModalProps {
    dueDateTime?: Date;
    folderId?: number;
    isModalOpen: boolean;
    hideModal: () => void;
}

const TaskCreationModal: React.FC<ITaskCreationModalProps> = observer(props => {
    const inboxId = -1;

    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [dueDateTime, setDueDateTime] = useState<Date | null>(
        props.dueDateTime ?? null
    );
    const [folderId, setFolderId] = useState<number>(props.folderId ?? inboxId);

    const { taskStore, folderStore } = useStore();

    const inboxOption = {
        key: inboxId,
        value: inboxId,
        label: "<Inbox>",
    };

    const folderOptions = folderStore.folders.map(f => ({
        key: f.id,
        value: f.id,
        label: f.title,
    }));

    const resetState = () => {
        setTitle("");
        setDescription("");
        setDueDateTime(null);
        setFolderId(inboxId);
    };

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleDescriptionChange = (
        event: React.ChangeEvent<HTMLTextAreaElement>
    ) => {
        setDescription(event.target.value);
    };

    const handleDueDateTimeChange = (date: Dayjs | null) => {
        setDueDateTime(date?.toDate() ?? null);
    };

    const handleFolderChange = (folderId: number) => {
        setFolderId(folderId);
    };

    const isCreateTaskButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleCreateTaskButtonClick = async () => {
        const normalizedFolderId = folderId === inboxId ? null : folderId;

        await taskStore.createTask(
            title,
            description,
            dueDateTime,
            normalizedFolderId
        );

        resetState();
        props.hideModal();
    };

    const handleCancelClick = () => {
        resetState();
        props.hideModal();
    };

    return (
        <Modal
            title="Новая задача"
            open={props.isModalOpen}
            okText="Создать задачу"
            onOk={handleCreateTaskButtonClick}
            okButtonProps={{
                disabled: isCreateTaskButtonDisabled(),
            }}
            cancelText="Отмена"
            onCancel={handleCancelClick}>
            <Space direction="vertical">
                <Input
                    placeholder="Название задачи"
                    value={title}
                    onChange={handleTitleChange}
                    style={{ width: 300 }}
                />
                <TextArea
                    placeholder="Описание задачи"
                    value={description}
                    onChange={handleDescriptionChange}
                    style={{ width: 300 }}
                />
                <DatePicker
                    placeholder="Дата"
                    value={dueDateTime !== null ? dayjs(dueDateTime) : null}
                    onChange={handleDueDateTimeChange}
                />
                <Select
                    placeholder="Папка"
                    options={[inboxOption, ...folderOptions]}
                    value={folderId}
                    onChange={handleFolderChange}
                    showSearch
                    optionFilterProp="label"
                    style={{ width: 300 }}
                />
            </Space>
        </Modal>
    );
});

export default TaskCreationModal;
