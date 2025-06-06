import React from "react";
import { Link, useLocation } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Dropdown, Menu, MenuProps } from "antd";
import { useStore } from "../stores/RootStore";
import IFolder from "../interfaces/IFolder";
import ITag from "../interfaces/ITag";
import FolderCreationModalButton from "./folder/FolderCreationModalButton";
import FolderUpdateModalMenuItem from "./folder/FolderUpdateModalMenuItem";
import TagCreationModalButton from "./tag/TagCreationModalButton";
import TagUpdateModalMenuItem from "./tag/TagUpdateModalMenuItem";
import TaskTag from "./tag/TaskTag";

type MenuItem = Required<MenuProps>["items"][number];

const MainMenu: React.FC = observer(() => {
    const location = useLocation();

    const { taskStore, folderStore, tagStore } = useStore();

    const allIncompletedTaskCount = taskStore.getIncompletedTasks().length;

    const todayIncompletedTaskCount =
        taskStore.getTodayIncompletedTasks().length;

    const inboxIncompletedTasksCount =
        taskStore.getInboxIncompletedTasks().length;

    const getLabel = (
        link: string,
        title: string,
        incompletedTaskCount?: number
    ) => {
        return (
            <Link to={link}>
                <div style={{ float: "left" }}>{title}</div>
                <div style={{ float: "right", color: "grey" }}>
                    {incompletedTaskCount}
                </div>
            </Link>
        );
    };

    const getFolderRootLabel = () => {
        return (
            <>
                <div style={{ float: "left" }}>Папки</div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ float: "right" }}>
                    <FolderCreationModalButton />
                </div>
            </>
        );
    };

    const getFolderLabel = (folder: IFolder) => {
        const folderIncompleteTaskCount = taskStore.getFolderIncompletedTasks(
            folder.id
        ).length;

        const handleDeleteFolderButtonClick = async () => {
            await folderStore.deleteFolder(folder);
        };

        const contextMenu = {
            items: [
                {
                    key: "updateFolder",
                    label: <FolderUpdateModalMenuItem folder={folder} />,
                },
                {
                    key: "deleteFolder",
                    label: "Удалить папку",
                    onClick: handleDeleteFolderButtonClick,
                },
            ],
        };

        return (
            <Dropdown
                key={folder.id}
                menu={contextMenu}
                trigger={["contextMenu"]}>
                {getLabel(
                    `/folders/${folder.id}`,
                    folder.title,
                    folderIncompleteTaskCount
                )}
            </Dropdown>
        );
    };

    const getFolderChildren = () => {
        return folderStore.getSortedFolders().map(f => {
            return {
                key: `/folders/${f.id}`,
                label: getFolderLabel(f),
            };
        });
    };

    const getTagRootLabel = () => {
        return (
            <>
                <div style={{ float: "left" }}>Теги</div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ float: "right" }}>
                    <TagCreationModalButton />
                </div>
            </>
        );
    };

    const getTagLabel = (tag: ITag) => {
        const tagIncompleteTaskCount = taskStore.getTagIncompletedTasks(
            tag.id
        ).length;

        const handleDeleteTagButtonClick = async () => {
            await tagStore.deleteTag(tag);
        };

        const contextMenu = {
            items: [
                {
                    key: "updateTag",
                    label: <TagUpdateModalMenuItem tag={tag} />,
                },
                {
                    key: "deleteTag",
                    label: "Удалить тег",
                    onClick: handleDeleteTagButtonClick,
                },
            ],
        };

        return (
            <Dropdown
                key={tag.id}
                menu={contextMenu}
                trigger={["contextMenu"]}>
                <Link to={`/tags/${tag.id}`}>
                    <div style={{ float: "left" }}>
                        <TaskTag tag={tag} />
                    </div>
                    <div style={{ float: "right", color: "grey" }}>
                        {tagIncompleteTaskCount}
                    </div>
                </Link>
            </Dropdown>
        );
    };

    const getTagChildren = () => {
        return tagStore.getSortedTags().map(t => {
            return {
                key: `/tags/${t.id}`,
                label: getTagLabel(t),
            };
        });
    };

    const mainMenuItems: MenuItem[] = [
        {
            key: "/all",
            label: getLabel("/all", "Все задачи", allIncompletedTaskCount),
        },
        {
            key: "/today",
            label: getLabel("/today", "Сегодня", todayIncompletedTaskCount),
        },
        {
            key: "/inbox",
            label: getLabel("/inbox", "Inbox", inboxIncompletedTasksCount),
        },
        {
            type: "divider",
        },
        {
            key: "/folders",
            label: getFolderRootLabel(),
            children: getFolderChildren(),
        },
        {
            type: "divider",
        },
        {
            key: "/tags",
            label: getTagRootLabel(),
            children: getTagChildren(),
        },
        {
            type: "divider",
        },
        {
            key: "/trash",
            label: getLabel("/trash", "Корзина"),
        },
    ];

    return (
        <Menu
            items={mainMenuItems}
            mode="inline"
            selectedKeys={[location.pathname]}
            defaultOpenKeys={["/folders"]}
        />
    );
});

export default MainMenu;
