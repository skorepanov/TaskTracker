import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Divider, Dropdown } from "antd";
import { useStore } from "../../stores/RootStore";
import { formatDateTime } from "../../utils";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import ITask from "../../interfaces/ITask";
import IFolder from "../../interfaces/IFolder";
import TaskTag from "../tag/TaskTag";

interface ITaskListItemProps {
    task: ITask;
    shouldShowFolder?: boolean;
    folder?: IFolder;
}

const TaskListItem: React.FC<ITaskListItemProps> = observer(props => {
    const { taskStore, tagStore } = useStore();

    const { task } = props;

    const [isCompleted, setIsCompleted] = useState<boolean>(
        task.completedDateTime !== null
    );

    const textColor = isCompleted ? "green" : "";

    const backgroundColor =
        taskStore.currentTask?.id === task.id ? "lightgray" : "";

    const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

    const taskTagComponents = taskTags
        ? taskTags.map(t => (
              <TaskTag
                  key={t.id}
                  tag={t}
              />
          ))
        : [];

    const folderComponent = props.shouldShowFolder ? (
        <>
            Папка: {props.folder?.title ?? "<Inbox>"}
            <br />
        </>
    ) : null;

    const dueDateTimeComponent =
        task.dueDateTime !== null ? (
            <>Срок выполнения: {formatDateTime(task.dueDateTime)}</>
        ) : null;

    const overdueDaysComponent =
        task.overdueDaysCount > 0 ? (
            <span style={{ color: isCompleted ? "" : "red" }}>
                ({task.overdueDaysCount} дней назад)
            </span>
        ) : null;

    const overdueComponent =
        task.dueDateTime !== null ? (
            <>
                {dueDateTimeComponent} {overdueDaysComponent}
                <br />
            </>
        ) : null;

    const handleTaskClick = () => {
        taskStore.setCurrentTask(task);
    };

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        setIsCompleted(isCompletedNew);

        if (isCompletedNew) {
            await taskStore.completeTask(task);
        } else {
            await taskStore.incompleteTask(task);
        }
    };

    const handleMoveToTrashButtonClick = async () => {
        await taskStore.moveTaskToTrash(task);
    };

    const contextMenu = {
        items: [
            {
                key: "moveToTrash",
                label: "Отправить в корзину",
                onClick: handleMoveToTrashButtonClick,
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
                    color: textColor,
                    backgroundColor: backgroundColor,
                }}>
                <Checkbox
                    checked={isCompleted}
                    onChange={handleCompletedChange}
                    style={{ marginRight: 5 }}
                />
                {task.title}
                <br />
                {taskTagComponents}
                {taskTagComponents.length > 0 ? <br /> : null}
                {folderComponent}
                {overdueComponent}
                <Divider size="small" />
            </div>
        </Dropdown>
    );
});

export default TaskListItem;
