import React from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Divider, Dropdown } from "antd";
import { useStore } from "../../stores/RootStore";
import { useSettings } from "../../contexts/SettingsContext";
import { useTranslation } from "../../hooks/useTranslation";
import { formatDate } from "../../utils";
import ITask from "../../interfaces/ITask";
import TaskTag from "../tag/TaskTag";

interface ITaskInTrashListItemProps {
    task: ITask;
}

const TaskInTrashListItem: React.FC<ITaskInTrashListItemProps> = observer(
    props => {
        const { taskStore, tagStore } = useStore();
        const { settings } = useSettings();
        const t = useTranslation();

        const { task } = props;

        const isCompleted = task.completedDateTime !== null;

        const selectedTaskColor =
            settings.theme === "dark" ? "#111a2c" : "#e6f4ff";
        const backgroundColor =
            taskStore.currentTask?.id === task.id ? selectedTaskColor : "";

        const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

        const taskTagComponents = taskTags
            ? taskTags.map(t => (
                  <TaskTag
                      key={t.id}
                      tag={t}
                  />
              ))
            : [];

        const dateComponent =
            task.completedDateTime !== null ? (
                formatDate(task.completedDateTime)
            ) : task.dueDateTime !== null ? (
                <span style={{ color: "red" }}>
                    {formatDate(task.dueDateTime)}
                </span>
            ) : null;

        const handleTaskClick = () => {
            taskStore.currentTaskId = task.id;
        };

        const handleRestoreTaskButtonClick = async () => {
            await taskStore.moveTaskFromTrash(task);
        };

        const handleDeleteTaskButtonClick = async () => {
            await taskStore.deleteTask(task);
        };

        const contextMenu = {
            items: [
                {
                    key: "restore",
                    label: t("restoreTask"),
                    onClick: handleRestoreTaskButtonClick,
                },
                {
                    key: "delete",
                    label: t("deleteTask"),
                    onClick: handleDeleteTaskButtonClick,
                },
            ],
        };

        return (
            <Dropdown
                key={task.id}
                menu={contextMenu}
                trigger={["contextMenu"]}>
                <div
                    onClick={handleTaskClick}
                    style={{
                        color: "grey",
                        backgroundColor: backgroundColor,
                        paddingLeft: 10,
                        paddingTop: 5,
                    }}>
                    <div style={{ float: "left" }}>
                        <Checkbox
                            checked={isCompleted}
                            disabled={true}
                            style={{ marginRight: 5 }}
                        />
                        {task.title}
                    </div>
                    <div style={{ float: "right" }}>{taskTagComponents}</div>
                    {dateComponent !== null ? (
                        <>
                            <br />
                            {dateComponent}
                        </>
                    ) : null}
                    <Divider size="small" />
                </div>
            </Dropdown>
        );
    }
);

export default TaskInTrashListItem;
